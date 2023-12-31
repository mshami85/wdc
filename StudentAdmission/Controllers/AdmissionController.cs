﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StudentAdmission.Classes;
using StudentAdmission.Data;
using StudentAdmission.Dtos;
using StudentAdmission.Models;

namespace StudentAdmission.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdmissionController : ControllerBase
    {
        DataContext _dataContext;
        AppSettings _appSettings;
        IMapper _mapper;
        public AdmissionController(IOptionsSnapshot<AppSettings> appSettings, DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        [HttpGet]
        [Consumes("application/json")]
        [ActionName("List")]
        public async Task<IActionResult> ListAsync()
        {
            try
            {
                var results = await _dataContext.Admissions.Include(ad => ad.Attachments).OrderByDescending(ad => ad.AdmissionDate).ToListAsync();
                var viewModels = results.Select(ad => _mapper.Map<AdmissionViewModel>(ad)).ToList();
                return Ok(viewModels);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [ActionName("Create")]
        //[Consumes("application/json", "application/octet-stream", "multipart/form-data")]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, MultipartBoundaryLengthLimit = int.MaxValue)]
        public async Task<IActionResult> CreateAsync([FromForm] AdmissionModel model)
        {
            string[] acceptedTypes =
            {
                "image/jpeg",
                "image/bmp",
                "image/png",
                "application/msword",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "application/pdf",
                "text/plain",
                ""
            };
            var files = Request.Form.Files;
            if ((files == null || files.Count == 0) && model == null)
            {
                return BadRequest("No data provided");
            }

            var attachmentsList = new List<AttachmentFile>();
            try
            {
                var folderPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, _appSettings.AttachmentsFolder ?? "Attachments"));
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                if (files != null && files.Count > 0)
                {
                    //if (files.Any(f => !acceptedTypes.Contains(f.ContentType)))
                    //{
                    //    return BadRequest("File types not accepted");
                    //}
                    foreach (var file in files)
                    {
                        var filePath = Path.Combine(folderPath, Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        attachmentsList.Add(new AttachmentFile
                        {
                            Name = Path.GetFileName(file.FileName),
                            Path = filePath,
                        });
                    }
                }

                var admission = _mapper.Map<Admission>(model);
                admission.Attachments = attachmentsList;
                admission.AdmissionDate = DateTime.Now;
                admission.Accepted = false;

                await _dataContext.Admissions.AddAsync(admission);
                await _dataContext.SaveChangesAsync();

                var viewModel = _mapper.Map<AdmissionViewModel>(admission);
                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                foreach (var file in attachmentsList)
                {
                    System.IO.File.Delete(file.Path);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, ex.GetDeepMessage());
            }
        }


        [HttpPut("{id}")]
        [ActionName("Accept")]
        public async Task<IActionResult> AcceptAdmission(int id)
        {
            try
            {
                var admission = await _dataContext.Admissions.FindAsync(id);
                if (admission == null)
                {
                    return NotFound("No admission");
                }

                admission.Accepted = true;

                _dataContext.Admissions.Update(admission);
                await _dataContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.GetDeepMessage());
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttachment(int id)
        {
            var att = await _dataContext.Attachments.FindAsync(id);

            if (att == null)
            {
                return BadRequest("Not found");
            }
            var fileInfo = new FileInfo(att.Path);
            if (!fileInfo.Exists || fileInfo.Length <= 0)
            {
                return NotFound();
            }

            return File(fileInfo.OpenRead(), "application/force-download", att.Name);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> IsAccepted([FromRoute] int id)
        {
            try
            {
                var stdAdmission = await _dataContext.Admissions.FindAsync(id);
                if (stdAdmission == null)
                {
                    return BadRequest("No student admission found");
                }
                return Ok(stdAdmission.Accepted);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.GetDeepMessage());
            }
        }
    }
}

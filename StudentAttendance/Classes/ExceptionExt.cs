using System.Text;

namespace StudentAttendance.Classes
{
    public static class ExceptionExt
    {
        public static string GetDeepMessage(this Exception ex)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var temp = ex;
            while (temp != null)
            {
                stringBuilder.AppendLine(temp.Message);
                temp = temp.InnerException;
            }
            return stringBuilder.ToString();
        }
    }
}

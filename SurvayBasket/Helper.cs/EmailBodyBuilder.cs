namespace SurvayBasket.Helper.cs
{
    public static class EmailBodyBuilder
    {
        
        public static string GenerateEmailBody(string template, Dictionary<string, string> templateModel)
        {

            var templatePath = $"{Directory.GetCurrentDirectory()}/Templates/{template}.html"; // تحديد مسار الملف داخل المشروع 

            var streamReader = new StreamReader(templatePath); // فتح ملف HTML للقراءة

            var body = streamReader.ReadToEnd(); // قراءة محتوى الملف بالكامل و تحويلة الي 
                                                 //string

            streamReader.Close(); // إغلاق الملف بعد القراءة

            // الصفحة فيها palce Holders 
            // هنا هيتم استبدالهم بقيم
            foreach (var item in templateModel)
                body = body.Replace(item.Key, item.Value);

            return body;


        }

    }
}

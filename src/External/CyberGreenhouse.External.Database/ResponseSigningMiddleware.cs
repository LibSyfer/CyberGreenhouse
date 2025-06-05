using CyberGreenhouse.Core;

namespace CyberGreenhouse.External.Database
{
    public class ResponseSigningMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SignatureService _signatureService;

        public ResponseSigningMiddleware(RequestDelegate next, SignatureService signatureService)
        {
            _next = next;
            _signatureService = signatureService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await _next(context);

            memoryStream.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

            string signature = _signatureService.SignData(responseBody);
            context.Response.Headers.Add("X-Api-Signature", signature);

            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.CopyToAsync(originalBodyStream);
        }
    }
}

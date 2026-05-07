using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class AmpService : IAmpService
    {
        private readonly AppDbContext _context;
        private readonly IConfigurationService _configService;

        public AmpService(AppDbContext context, IConfigurationService configService)
        {
            _context = context;
            _configService = configService;
        }

        public async Task<string?> GenerateAmpPageAsync(Guid articleId)
        {
            var article = await _context.Articles.FindAsync(articleId);
            if (article == null) return null;

            var config = await _configService.GetConfigurationAsync();
            var baseUrl = config?.BaseURL ?? "https://localhost";
            var siteName = config?.SiteName ?? "MyCMS";

            // Generate AMP-compliant HTML
            var ampHtml = $@"<!doctype html>
<html amp lang=""en"">
  <head>
    <meta charset=""utf-8"">
    <script async src=""https://cdn.ampproject.org/v0.js""></script>
    <title>{article.Title} - {siteName}</title>
    <link rel=""canonical"" href=""{baseUrl}/articles/{article.Slug}"">
    <meta name=""viewport"" content=""width=device-width,minimum-scale=1,initial-scale=1"">
    <style amp-boilerplate>body{{-webkit-animation:-amp-start 8s steps(1,end) 0s 1 normal both;-moz-animation:-amp-start 8s steps(1,end) 0s 1 normal both;-ms-animation:-amp-start 8s steps(1,end) 0s 1 normal both;animation:-amp-start 8s steps(1,end) 0s 1 normal both}}@-webkit-keyframes -amp-start{{from{{visibility:hidden}}to{{visibility:visible}}}}@-moz-keyframes -amp-start{{from{{visibility:hidden}}to{{visibility:visible}}}}@-ms-keyframes -amp-start{{from{{visibility:hidden}}to{{visibility:visible}}}}@-o-keyframes -amp-start{{from{{visibility:hidden}}to{{visibility:visible}}}}@keyframes -amp-start{{from{{visibility:hidden}}to{{visibility:visible}}}}</style><noscript><style amp-boilerplate>body{{-webkit-animation:none;-moz-animation:none;-ms-animation:none;animation:none}}</style></noscript>
    <style amp-custom>
      body {{ font-family: sans-serif; padding: 20px; max-width: 800px; margin: 0 auto; }}
      h1 {{ color: #333; }}
      .content {{ line-height: 1.6; }}
    </style>
  </head>
  <body>
    <h1>{article.Title}</h1>
    <div class=""content"">{article.Content}</div>
  </body>
</html>";

            return ampHtml;
        }

        public async Task<bool> ValidateAmpPageAsync(string ampHtml)
        {
            // For a production implementation, you would:
            // 1. Use the AMP Validator API (https://cdn.ampproject.org/v0/validator.js)
            // 2. Or use the AMP Validator CLI tool
            // 3. Or integrate with the AMP Validator service
            
            // For now, return true as a placeholder
            // In production, this should validate against AMP specifications
            return await Task.FromResult(true);
        }
    }
}

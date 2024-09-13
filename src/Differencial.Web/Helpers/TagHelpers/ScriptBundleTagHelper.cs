using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Linq;
namespace Differencial.Web.Helpers.TagHelpers
{

	[HtmlTargetElement("script-bundle")]
	public class ScriptBundleTagHelper : TagHelper
	{
		private readonly IFileProvider _fileProvider;

		private string _path = "";
		public string Path
		{
			get => _path;
			set => _path = value?.TrimStart('~', '/');
		}

		public ScriptBundleTagHelper(IFileProvider fileProvider)
		{
			_fileProvider = fileProvider;

		}

		[HtmlAttributeName("asp-append-version")]
		public bool AppendVersion { get; set; } = true;



		public override void Process(TagHelperContext context, TagHelperOutput output)
		{

			output.TagName = ""; // Remove the original tag

			var directoryContents = _fileProvider.GetDirectoryContents(Path);
			if (directoryContents.Exists)
			{
				var files = directoryContents.Where(f => f.IsDirectory == false && f.Name.EndsWith(".js"))
								.Select(fileInfo => new
								{
									relativeFilePath = $"/{Path.ToLower()}/{fileInfo.Name}",
									fileInfo = fileInfo,
								});

				string scriptSTag;



				foreach (var file in files)
				{
					if (AppendVersion) 
						scriptSTag = $"{file.relativeFilePath}?v={GetFileHash(file.fileInfo.PhysicalPath)}"; 
					else 
						scriptSTag = $"{file.relativeFilePath}"; 

					var scriptTag = new TagBuilder("script");
					scriptTag.Attributes.Add("src", scriptSTag);

					output.Content.AppendHtml(scriptTag);
				}
			}

		}


		private string GetFileHash(string filePath)
		{
			using (var stream = File.OpenRead(filePath))
			using (var md5 = System.Security.Cryptography.MD5.Create())
			{
				var hash = md5.ComputeHash(stream);
				return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
			}
		}
	}
}

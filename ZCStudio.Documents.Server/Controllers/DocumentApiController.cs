﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Linq;
using ZCStudio.Documents.Server.Models;
using ZCStudio.Documents.Server.Tools;

namespace ZCStudio.Documents.Server.Controllers
{
    [Produces("application/json")]
    [Route("Document/api")]
    public class DocumentApiController : Controller
    {
        private DocsConfig docsConfig;

        public DocumentApiController(IOptionsSnapshot<DocsConfig> docsConfigOptionsAccessor)
        {
            docsConfig = docsConfigOptionsAccessor.Value;
        }

        // GET: api/DocApi
        [HttpGet]
        public IActionResult Get()
        {
            var docpath = docsConfig.GetDocPath();
            if (!Directory.Exists(docpath))
            {
                return Json(new { IsSuccess = false });
            }
            var root = BuildTree(new DirectoryInfo(docpath));

            return Json(new DocTreeNode[] { root }, new Newtonsoft.Json.JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        // GET: api/DocApi/5
        [HttpGet("{docName}", Name = "Get")]
        public IActionResult Get(string docName)
        {
            var docpath = docsConfig.GetDocPath(docName);
            if (!Directory.Exists(docpath))
            {
                return Json(new { IsSuccess = false });
            }
            var root = BuildTree(new DirectoryInfo(docpath));

            return Json(new DocTreeNode[] { root }, new Newtonsoft.Json.JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet("GetFileText/{path}")]
        public IActionResult GetFileText(string path)
        {
            var docpath = docsConfig.GetDocPath(path);
            if (!System.IO.File.Exists(docpath))
            {
                return Json(new { IsSuccess = false });
            }

            var mdfiletext = System.IO.File.ReadAllText(docpath);

            return Json(new { IsSuccess = true, result = mdfiletext, extension = Path.GetExtension(docpath) });
        }

        [HttpGet("GetFile/{path}")]
        public IActionResult GetFile(string path)
        {
            var docpath = docsConfig.GetDocPath(path);
            if (!System.IO.File.Exists(docpath))
            {
                return NotFound();
            }
            return PhysicalFile(docpath, "application/pdf");
        }

        // POST: api/DocApi
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        private DocTreeNode BuildTree(DirectoryInfo directory)
        {
            if (null == directory)
            {
                return null;
            }
            DocTreeNode root = new DocTreeNode(docsConfig.GetDocPath(), directory);
            foreach (var file in directory.GetFileSystemInfos().OrderBy(i => i.Name, new FileNameComparer()))
            {
                if (file is FileInfo)
                {
                    root.AppendNode(new DocTreeNode(docsConfig.GetDocPath(), file));
                }
                else
                {
                    var dnode = BuildTree((DirectoryInfo)file);
                    if (null != dnode)
                    {
                        root.AppendNode(dnode);
                    }
                }
            }
            return root;
        }
    }
}
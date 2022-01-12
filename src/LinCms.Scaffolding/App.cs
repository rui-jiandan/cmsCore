﻿using Humanizer;
using LinCms.Scaffolding.Entities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LinCms.Scaffolding
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly SettingOptions _settingOptions;
        public App(ILogger<App> logger,IOptionsMonitor<SettingOptions> settingOptions)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingOptions = settingOptions.CurrentValue;
        }

        public async Task RunAsync(string[] args)
        {
            _logger.LogInformation("Starting...");

            ProjectInfo projectInfo = ProjectParser();

            string entityPath = Path.Combine(_settingOptions.BaseDirectory, _settingOptions.EntityFilePath);
            EntityInfo entityInfo = EntityParse(entityPath, projectInfo);

            var model = new
            {
                ProjectInfo = projectInfo,
                EntityInfo = entityInfo,
                Option = new CommandOption()
                {
                    CustomRepository = false,
                },
                SettingOptions = _settingOptions
            };
            #region 参考值
            /*
                -   ProjectInfo	{LinCms.Scaffolding.Entities.ProjectInfo}	LinCms.Scaffolding.Entities.ProjectInfo
                    BaseDirectory	"D:/code/github/lin-cms-dotnetcore/src/LinCms.Core/"	string
                    FullName	"LinCms"	string
                    Name	"LinCms"	string

                -   EntityInfo
                    BaseType	"FullAduitEntity"	string
                    Name	"Doc"	string		
                    NameCamelize	"doc"	string
                    NamePluralized	"Docs"	string
                    Namespace	"LinCms.v1.Docs"	string
                    NamespaceLastPart	"Docs"	string
                    PrimaryKey	"long"	string
                    RelativeDirectory	"v1/Docs"	string
                    RelativeNamespace	"v1.Docs"	string

                -   SettingOptions	{LinCms.Scaffolding.SettingOptions}	LinCms.Scaffolding.SettingOptions
                    Areas	"Base"	string
                    AreasCamelize	"base"	string
                    BaseDirectory	"D:/code/github/lin-cms-dotnetcore/src/LinCms.Core/"	string
                    EntityFilePath	"Entities/Doc.cs"	string
                    OutputDirectory	"D:/code/github/Outputs"	string
                    ProjectName	"LinCms"	string
                    TemplatePath	"./Templates"	string

            */
            #endregion
            string templatePath = Path.Combine(Environment.CurrentDirectory, _settingOptions.TemplatePath);
            CodeScaffolding codeScaffolding = new CodeScaffolding(templatePath, _settingOptions.OutputDirectory, _logger);
            await codeScaffolding.GenerateAsync(model);

            _logger.LogInformation("Finished!");
        }

        private ProjectInfo ProjectParser()
        {
            _logger.LogInformation($"baseDirectory：{_settingOptions.BaseDirectory}"); ;
            string coreCsprojFile = Directory.EnumerateFiles(_settingOptions.BaseDirectory, "*.Core.csproj", SearchOption.AllDirectories).FirstOrDefault();

            string fileName = Path.GetFileName(coreCsprojFile);
            string fullName = fileName.RemovePostFix(".Core.csproj");
            ProjectInfo projectInfo = new ProjectInfo(_settingOptions.BaseDirectory, fullName);

            return projectInfo;
        }

        private EntityInfo EntityParse(string entityFilePath, ProjectInfo projectInfo)
        {
            string sourceText = File.ReadAllText(entityFilePath);

            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceText);

            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            string @namespace = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().Single().Name.ToString();//不满足项目命名空间
            ClassDeclarationSyntax classDeclarationSyntax = root.DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
            string className = classDeclarationSyntax.Identifier.ToString();
            BaseListSyntax baseList = classDeclarationSyntax.BaseList;
            GenericNameSyntax genericNameSyntax = baseList.DescendantNodes().OfType<SimpleBaseTypeSyntax>()
                .First(node => !node.ToFullString().StartsWith("I")) // Not interface
                .DescendantNodes().OfType<GenericNameSyntax>()
                .FirstOrDefault();

            string baseType;
            string primaryKey;
            if (genericNameSyntax == null)
            {
                // No generic parameter -> Entity with Composite Keys
                baseType = baseList.DescendantNodes().OfType<SimpleBaseTypeSyntax>().Single().Type.ToString();
                primaryKey = "long";

            }
            else
            {
                // Normal entity
                baseType = genericNameSyntax.Identifier.ToString();
                primaryKey = genericNameSyntax.DescendantNodes().OfType<TypeArgumentListSyntax>().Single().Arguments[0].ToString();
            }
            List<PropertyInfo> properties = root.DescendantNodes().OfType<PropertyDeclarationSyntax>()
                  .Select(prop =>

                        new PropertyInfo(prop.Type.ToString(), prop.Identifier.Value.ToString())
                  )
                  .ToList();

            string xmlPath = _settingOptions.BaseDirectory + projectInfo.FullName + ".Core.xml";
            string entityRemark = Util.GetEntityRemarkBySummary(xmlPath, properties, @namespace + "." + className);


            if (_settingOptions.Areas != null)
            {
                @namespace = projectInfo.FullName + "." + _settingOptions.Areas + "." + className.Pluralize();
            }
            else
            {
                @namespace = projectInfo.FullName + "." + className.Pluralize();
            }
            string relativeDirectory = @namespace.RemovePreFix(projectInfo.FullName + ".").Replace('.', '/');

            EntityInfo entityInfo = new EntityInfo(@namespace, className, baseType, primaryKey, relativeDirectory);
            entityInfo.Properties.AddRange(properties);
            entityInfo.EntityRemark = entityRemark;

            return entityInfo;
        }
    }
}

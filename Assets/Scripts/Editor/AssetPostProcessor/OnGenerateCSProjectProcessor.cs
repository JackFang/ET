using System;
using UnityEditor;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Text;

namespace ET
{
    public class OnGenerateCSProjectProcessor : AssetPostprocessor
    {
        public static string OnGeneratedCSProject(string path, string content)
        {
            
            if (path.EndsWith("Client.Hotfix.csproj"))
            {
                content =  content.Replace("<Compile Include=\"Assets\\Scripts\\Client\\Hotfix\\Empty.cs\" />", string.Empty);
                content =  content.Replace("<None Include=\"Assets\\Scripts\\Client\\Hotfix\\Client.Hotfix.asmdef\" />", string.Empty);
            }
            
            if (path.EndsWith("Client.HotfixView.csproj"))
            {
                content =  content.Replace("<Compile Include=\"Assets\\Scripts\\Client\\HotfixView\\Empty.cs\" />", string.Empty);
                content =  content.Replace("<None Include=\"Assets\\Scripts\\Client\\HotfixView\\Client.HotfixView.asmdef\" />", string.Empty);
            }
            
            if (path.EndsWith("Client.Model.csproj"))
            {
                content =  content.Replace("<Compile Include=\"Assets\\Scripts\\Client\\Model\\Empty.cs\" />", string.Empty);
                content =  content.Replace("<None Include=\"Assets\\Scripts\\Client\\Model\\Client.Model.asmdef\" />", string.Empty);
            }
            
            if (path.EndsWith("Client.ModelView.csproj"))
            {
                content =  content.Replace("<Compile Include=\"Assets\\Scripts\\Client\\ModelView\\Empty.cs\" />", string.Empty);
                content =  content.Replace("<None Include=\"Assets\\Scripts\\Client\\ModelView\\Client.ModelView.asmdef\" />", string.Empty);
            }
            
            if (path.EndsWith("Server.Hotfix.csproj"))
            {
                content =  content.Replace("<Compile Include=\"Assets\\Scripts\\Server\\Hotfix\\Empty.cs\" />", string.Empty);
                content =  content.Replace("<None Include=\"Assets\\Scripts\\Server\\Hotfix\\Server.Hotfix.asmdef\" />", string.Empty);
            }
            
            if (path.EndsWith("Server.Model.csproj"))
            {
                content =  content.Replace("<Compile Include=\"Assets\\Scripts\\Server\\Model\\Empty.cs\" />", string.Empty);
                content =  content.Replace("<None Include=\"Assets\\Scripts\\Server\\Model\\Server.Model.asmdef\" />", string.Empty);
            }
            
            if (path.EndsWith("Client.Hotfix.csproj"))
            {
                return GenerateCustomProject(path, content, @"Codes\\Client\Hotfix\**\*.cs", @"Codes\\Share\Hotfix\**\*.cs");
            }

            if (path.EndsWith("Client.HotfixView.csproj"))
            {
                return GenerateCustomProject(path, content, @"Codes\\Client\HotfixView\**\*.cs");
            }

            if (path.EndsWith("Client.Model.csproj"))
            {
                return GenerateCustomProject(path, content, @"Codes\\Client\Model\**\*.cs", @"Codes\\Share\Model\**\*.cs");
            }

            if (path.EndsWith("Client.ModelView.csproj"))
            {
                return GenerateCustomProject(path, content, @"Codes\\Client\ModelView\**\*.cs");
            }
            
            if (path.EndsWith("Server.Model.csproj"))
            {
                return GenerateCustomProject(path, content, @"Codes\\Server\Model\**\*.cs", @"Codes\\Client\Model\**\*.cs", @"Codes\\Share\Model\**\*.cs");
            }
            
            if (path.EndsWith("Server.Hotfix.csproj"))
            {
                return GenerateCustomProject(path, content, @"Codes\\Server\Hotfix\**\*.cs", @"Codes\\Client\Hotfix\**\*.cs", @"Codes\\Share\Hotfix\**\*.cs");
            }
            
            return content;
        }

        private static string GenerateCustomProject(string path, string content, params string[] codesPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);

            var newDoc = doc.Clone() as XmlDocument;

            var rootNode = newDoc.GetElementsByTagName("Project")[0];

            var itemGroup = newDoc.CreateElement("ItemGroup", newDoc.DocumentElement.NamespaceURI);
            foreach (string s in codesPath)
            {
                var compile = newDoc.CreateElement("Compile", newDoc.DocumentElement.NamespaceURI);
                compile.SetAttribute("Include", s);
                itemGroup.AppendChild(compile);
            }

            

            var projectReference = newDoc.CreateElement("ProjectReference", newDoc.DocumentElement.NamespaceURI);
            projectReference.SetAttribute("Include", @"..\Share\Analyzer\Share.Analyzer.csproj");
            projectReference.SetAttribute("OutputItemType", @"Analyzer");
            projectReference.SetAttribute("ReferenceOutputAssembly", @"false");

            var project = newDoc.CreateElement("Project", newDoc.DocumentElement.NamespaceURI);
            project.InnerText = @"{d1f2986b-b296-4a2d-8f12-be9f470014c3}";
            projectReference.AppendChild(project);

            var name = newDoc.CreateElement("Name", newDoc.DocumentElement.NamespaceURI);
            name.InnerText = "Analyzer";
            projectReference.AppendChild(project);

            itemGroup.AppendChild(projectReference);

            rootNode.AppendChild(itemGroup);

            using (StringWriter sw = new StringWriter())
            {

                using (XmlTextWriter tx = new XmlTextWriter(sw))
                {
                    tx.Formatting = Formatting.Indented;
                    newDoc.WriteTo(tx);
                    tx.Flush();
                    return sw.GetStringBuilder().ToString();
                }
            }
        }
    }
}
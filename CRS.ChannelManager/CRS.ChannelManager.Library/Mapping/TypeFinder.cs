using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.Mapping
{
    public class TypeFinder
    {
        public static Type? FindType(string typeName, string? namespaceProject = "domain.entities")
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            string projectRef = string.Empty;
            string folderRef = string.Empty;
            string typeNameStart = string.Empty;
            List<string> listNamespace = new List<string>();
            if (!string.IsNullOrEmpty(namespaceProject))
            {
                listNamespace = namespaceProject.Split(".").ToList();
                projectRef = namespaceProject.Split(".").First();
                if (listNamespace.Count > 1)
                {
                    folderRef = listNamespace.Last();
                }
                assemblies = assemblies.Where(t => !string.IsNullOrEmpty(t.FullName) && t.FullName.Contains(projectRef, StringComparison.OrdinalIgnoreCase)).ToArray();
                typeName = $"{namespaceProject}.{typeName}";
            }
            foreach (var assembly in assemblies)
            {
                if (assembly != null && assembly.GetExportedTypes().Any())
                {
                    var type = assembly.GetExportedTypes().FirstOrDefault(t => !string.IsNullOrEmpty(t.FullName) && t.FullName.Contains(typeName, StringComparison.OrdinalIgnoreCase));
                    if (type != null)
                    {
                        return type;
                    }
                }
                return null;
            }
            return null;
        }
    }
}

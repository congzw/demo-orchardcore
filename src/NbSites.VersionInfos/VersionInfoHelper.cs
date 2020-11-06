using System;
using System.IO;
using Newtonsoft.Json;

namespace NbSites.VersionInfos
{
    public interface IVersionInfoHelper
    {
        ArtifactInfo TryFindArtifactInfo();
    }

    public class VersionInfoHelper : IVersionInfoHelper
    {
        public ArtifactInfo TryFindArtifactInfo()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"artifact.json");
            if (!File.Exists(filePath))
            {
                return null;
            }

            var json = File.ReadAllText(filePath);
            var artifactInfo = JsonConvert.DeserializeObject<ArtifactInfo>(json);
            return artifactInfo;
        }

        #region for extensions
        
        public static Func<IVersionInfoHelper> Instance = () => Lazy.Value;

        private static readonly Lazy<VersionInfoHelper> Lazy = new Lazy<VersionInfoHelper>(() => new VersionInfoHelper());

        #endregion
    }

    public static class VersionInfoHelperExtensions
    {
        public static VersionInfo GetVersionInfo(this IVersionInfoHelper helper)
        {
            var versionInfo = VersionInfo.Instance;
            var artifactInfo = helper.TryFindArtifactInfo();
            if (artifactInfo != null)
            {
                versionInfo.ArtifactInfo = artifactInfo;
            }

            return versionInfo;
        }
    }
}
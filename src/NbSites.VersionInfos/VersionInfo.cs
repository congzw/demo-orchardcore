using System;
using System.Collections.Generic;
using System.Linq;

namespace NbSites.VersionInfos
{
    /// <summary>
    /// 版本信息
    /// </summary>
    public class VersionInfo
    {
        public VersionInfo()
        {
            this.LastUpdateAt = new DateTime(2020, 11, 1);
            this.CurrentVersion = Version.Parse("0.1.0");
            VersionInfoRegistry.Init(this);
        }

        /// <summary>
        /// 最后的更改时间
        /// </summary>
        public DateTime LastUpdateAt { get; set; }
        /// <summary>
        /// 当前版本
        /// </summary>
        public Version CurrentVersion { get; set; }
        /// <summary>
        /// 产品变更日志列表
        /// </summary>
        public IList<ChangeLog> ChangeLogs { get; set; } = new List<ChangeLog>();
        /// <summary>
        /// 制品库信息
        /// </summary>
        public ArtifactInfo ArtifactInfo { get; set; }
        /// <summary>
        /// GetFeatures by version
        /// </summary>
        /// <param name="sinceVersion"></param>
        /// <returns></returns>
        public IList<ChangeLog> GetChangeLogs(Version sinceVersion)
        {
            var query = ChangeLogs as IEnumerable<ChangeLog>;
            if (sinceVersion != null)
            {
                query = query.Where(x => x.SinceVersion >= sinceVersion);
            }
            return query.OrderByDescending(x => x.SinceVersion).ToList();
        }

        public ChangeLog AppendChangeLog(string title, string description, string since, DateTime completedAt)
        {
            var changeLog = new ChangeLog()
            {
                Title = title,
                Description = description,
                SinceVersion = Version.Parse(since),
                CompletedAt = completedAt
            };

            this.ChangeLogs.Add(changeLog);

            var theLastOne = this.ChangeLogs.OrderBy(x => x.CompletedAt).LastOrDefault();
            if (theLastOne != null)
            {
                this.LastUpdateAt = theLastOne.CompletedAt;
            }

            var theMaxVersion = this.ChangeLogs.OrderBy(x => x.SinceVersion).LastOrDefault();
            if (theMaxVersion != null)
            {
                this.CurrentVersion = theMaxVersion.SinceVersion;
            }

            return changeLog;
        }
        

        public static readonly VersionInfo Instance = new VersionInfo();
    }

    /// <summary>
    /// 制品信息
    /// </summary>
    public class ArtifactInfo
    {
        public string ArtifactId { get; set; }
        public string Project { get; set; }
        public string Branch { get; set; }
        public string Runtime { get; set; }
        public string BuildNo { get; set; }
        public string JobName { get; set; }
        public string FileName { get; set; }
        public string SaveLocation { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// 产品变更日志
    /// </summary>
    public class ChangeLog
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 完成的版本
        /// </summary>
        public Version SinceVersion { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime CompletedAt { get; set; }

        /// <summary>
        /// 记录项
        /// </summary>
        public List<string> Items { get; set; } = new List<string>();

        /// <summary>
        /// 增加条目
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public ChangeLog With(string content)
        {
            Items.Add(content);
            return this;
        }
    }
}

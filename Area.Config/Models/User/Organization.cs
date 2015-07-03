using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNetIdentityRoles.Areas.Config.Models.User
{
    public class Organization
    {
        [Key]
        [Display(Name = "主键")]
        public string LINEID { get; set; }

        [MaxLength(20)]
        [Display(Name = "状态")]
        public string STATUS { get; set; }

        [MaxLength(50)]
        [Display(Name = "创建人")]
        public string CRMAN { get; set; }

        [Display(Name = "创建时间")]
        public Nullable<DateTime> CRTIME { get; set; }

        [MaxLength(50)]
        [Display(Name = "备注")]
        public string MEMO { get; set; }

        [Display(Name = "编号")]
        public int ID { get; set; }

        [MaxLength(20)]
        [Display(Name = "名称")]
        public string NAME { get; set; }

        [Display(Name = "父编号")]
        public int PID { get; set; }

        [Display(Name = "层次")]
        public int LVL { get; set; }

        [MaxLength(50)]
        [Display(Name = "编号2")]
        public string ID2 { get; set; }

        [MaxLength(50)]
        [Display(Name = "顺序号")]
        public string ORDERNO { get; set; }

        [MaxLength(20)]
        [Display(Name = "旧编号")]
        public string OLDID { get; set; }

    }

    public class OrganizationDbContext : DbContext
    {
        public OrganizationDbContext()
            : base("name=DefaultConnection")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //重新配置表名和方案名
            modelBuilder.Entity<Organization>().ToTable("TB_SYS_USER_ORGS", "LDJX");

        }
        public virtual DbSet<Organization> Entities { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IRIT.Framework.Resources;

namespace IRIT.EdMS.Common.Model.ViewModel.SchoolsGroup
{
    public class SchoolsGroupViewModel
    {
        public long Id { get; set; }

        //[Required(ErrorMessageResourceName = "EnterInfraStructureRingCode", ErrorMessageResourceType = typeof(GeneralResource))]
        [Display(Name = "SchoolsGroupCode", ResourceType = typeof(GeneralResource))]
        public string Code { get; set; }

        [Required(ErrorMessageResourceName = "EnterSchoolsGroupName", ErrorMessageResourceType = typeof(GeneralResource))]
        [Display(Name = "SchoolsGroupName", ResourceType = typeof(GeneralResource))]
        [StringLength(256, MinimumLength = 3, ErrorMessageResourceType = typeof(GeneralResource), ErrorMessageResourceName = "StringLength3To256")]
        public string SchoolsGroupName { get; set; }

        #region Comment
        [Display(Name = "Comment", ResourceType = typeof(GeneralResource))]
        public string Comment { get; set; }
        #endregion

        #region InfraStructureRingKindID
        [Required(ErrorMessageResourceName = "EnterInfraStructureRing", ErrorMessageResourceType = typeof(GeneralResource))]
        [Display(Name = "InfraStructureRingName", ResourceType = typeof(GeneralResource))]
        public long InfraStructureRingId { get; set; }
        #endregion

        #region InfraStructureRingName
        [Display(Name = "InfraStructureRingKindTitle", ResourceType = typeof(GeneralResource))]
        public string InfraStructureRingName { get; set; }
        #endregion

        public List<Framework.DataModel.Common.School> Schools { get; set; }
    }
}

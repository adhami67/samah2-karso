using System.ComponentModel.DataAnnotations;
using IRIT.Framework.Resources;

namespace IRIT.EdMS.Common.Model.ViewModel.InfraStructureRings
{
    public class InfraStructureRingViewModel
    {
        public long Id { get; set; }

        //[Required(ErrorMessageResourceName = "EnterInfraStructureRingCode", ErrorMessageResourceType = typeof(GeneralResource))]
        [Display(Name = "InfraStructureRingCode", ResourceType = typeof(GeneralResource))]
        public string Code { get; set; }

        [Required(ErrorMessageResourceName = "EnterInfraStructureRing", ErrorMessageResourceType = typeof(GeneralResource))]
        [Display(Name = "InfraStructureRingName", ResourceType = typeof(GeneralResource))]
        [StringLength(256, MinimumLength = 3, ErrorMessageResourceType = typeof(GeneralResource), ErrorMessageResourceName = "StringLength3To256")]
        public string InfraStructureRingName { get; set; }

        #region Comment
        [Display(Name = "Comment", ResourceType = typeof(GeneralResource))]
        public string Comment { get; set; }
        #endregion

        #region InfraStructureRingKindID
        [Required(ErrorMessageResourceName = "EnterInfraStructureRingKindID", ErrorMessageResourceType = typeof(GeneralResource))]
        [Display(Name = "InfraStructureRingKindID", ResourceType = typeof(GeneralResource))]
        public long InfraStructureRingKindId { get; set; }
        #endregion

        #region InfraStructureRingKindTitle
        [Display(Name = "InfraStructureRingKindID", ResourceType = typeof(GeneralResource))]
        public string InfraStructureRingKindTitle { get; set; }
        #endregion
    }
}

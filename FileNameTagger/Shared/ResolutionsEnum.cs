using System.ComponentModel;

namespace Shared
{
    public enum ResolutionsEnum
    {
        SD = 1,
        HD = 2,
        FHD = 3,
        [Description("1440p")]
        QHD = 4,
        UHD = 5
    }
}

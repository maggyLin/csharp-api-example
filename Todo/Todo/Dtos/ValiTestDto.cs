using System;
using System.ComponentModel.DataAnnotations;
using Todo.ValidationAttributes;

namespace Todo.Dtos
{
    //自定義資料驗證(非單一值)
    [StartAndEndTime]
    public class ValiTestDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }

}

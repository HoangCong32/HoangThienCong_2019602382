using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WebBanSach.Data.Enums
{
    public enum BillStatus
    {
        [Description("Hóa đơn mới")]
        New,
        [Description("Đang giao hàng")]
        InProgress,
        [Description("Gửi trả")]
        Returned,
        [Description("Hủy đơn")]
        Cancelled,
        [Description("Hoàn thành")]
        Completed
    }
}

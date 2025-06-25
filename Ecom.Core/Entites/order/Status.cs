using System.Runtime.Serialization;

namespace Ecom.Core.Entites.order
{
    public enum Status
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "PaymentReceived")]
        PaymentReceived,
        [EnumMember(Value = "PaymentFaild")]
        PaymentFaild
    }
}
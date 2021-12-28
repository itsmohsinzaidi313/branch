using System.Runtime.Serialization;

namespace Branch.Exceptions
{
    internal static class ExceptionMessages
    {
        internal const string
            OrderSavingProblem = "Unable to save order.",
            MissingOrderNo = "Order number or one of its required property is missing.",
            MissingTokenNo = "Token number or one of its required property is missing.",
            MissingOrderDate = "Order date or one of its required property is missing.",
            MissingCustomer = "Customer or one of its required property is missing.",
            MissingWaiter = "Waiter or one of its required property is missing.",
            MissingRider = "Rider or one of its required property is missing.",
            MissingTable = "Table or one of its required property is missing.",
            MissingOrderType = "Order type or one of its required property is missing.",
            MissingPaymentMode = "Payment mode or one of its required property is missing.",
            MissingCounter = "Counter or one of its required property is missing.",
            MissingUser = "User or one of its required property is missing.",
            MissingNoOfPersons = "Number of persons or one of its required property is missing.",
            MissingShift = "Shift  or one of its required property is missing.",
            MissingItems = "Items are missing.",
            InvalidDiscount = "Discount or one of its required property is invalid.",
            InvalidTax = "Tax has as invalid value.",
            InvalidItem = "Item or one of its required property is invalid.",
            InvalidDeal = "Deal or one of its required property is invalid.",
            InvalidOrderDate = "Order date is invalid.",
            ErrorSetItemQuantity = "Exception occured while setting quantity. ItemId or quantity is invalid.";

    }
}

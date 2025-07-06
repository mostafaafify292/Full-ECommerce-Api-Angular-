export interface ICreateOrder {
    deliveryMethodID: number;
    basketId: string;
    shipAddress: IShippingAddress;
}
  
export interface IShippingAddress {
    firstName: string;
    lastName: string;
    street: string;
    city: string;
    zipCode: string;
    state: string;
}
  export interface IOrder {
    id: number;
    buyerEmail: string;
    supTotal: number;
    total: number;
    orderDate: Date;
    shippingAddress: IShippingAddress;
    deliveryMethod: string;
    orderItems: IOrderItem[];
    status: string;
}

export interface IOrderItem {
    productItemId: number;
    mainImage: string;
    productName: string;
    price: number;
    quantity: number;
}
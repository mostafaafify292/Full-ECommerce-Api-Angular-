import { v4 as uuidv4 } from 'uuid';

export interface IBasket {
  id: string;
  paymentIntentId: string;
  clientSecret: string;
  basketItems: IBasketItem[];
}

export interface IBasketItem {
  id: number;
  name: string;
  description: string;
  quantity: number;
  imageURL: string;
  price: number;
  category: string;
}
export class Basket implements IBasket {
  paymentIntentId: string;
  clientSecret: string;
  id = uuidv4();
  basketItems: IBasketItem[] = [];
}
export interface IBasketTotal{
  shipping:number;
  subTotal:number;
  total:number;
}

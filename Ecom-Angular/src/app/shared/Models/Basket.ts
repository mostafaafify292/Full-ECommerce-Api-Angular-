import { v4 as uuidv4 } from 'uuid';

export interface IBasket {
  Id: string;
  basketItems: IBasketItem[];
}

export interface IBasketItem {
  Id: string;
  Name: string;
  Quantity: number;
  ImageURL: string;
  Price: number;
  Category: string;
}
export class Basket implements IBasket {
  Id = uuidv4();
  basketItems: IBasketItem[]=[];
}

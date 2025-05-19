export interface IProduct {
  name: string;
  description: string;
  newPrice: number;
  oldPrice: number;
  photos: IPhoto[];
  categoryName: string;
}

export interface IPhoto {
  imageName: string;
  productId: number;
}

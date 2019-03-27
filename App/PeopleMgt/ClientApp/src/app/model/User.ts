export class User {
  public id: number;
  public firstname: string;
  public lastname: string;
  public email: string;
  public interests: string;
  public address: string;
  public age: number; 
  public picture: string;

  public constructor(init?: Partial<User>) {
    (<any>Object).assign(this, init);
  }
}

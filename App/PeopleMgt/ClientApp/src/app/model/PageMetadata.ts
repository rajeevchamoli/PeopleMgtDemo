export class PageMetadata {
  public Current: string;
  public Previous: boolean;
  public Next: boolean;
  public Size: string;
  public PageCount: number;
  public RecordCount: number;
  
  public constructor(init?: Partial<PageMetadata>) {
    (<any>Object).assign(this, init);    
  }
}

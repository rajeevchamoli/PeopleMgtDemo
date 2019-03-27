export class PageParameters {
  public filter: string;
  public sortOrder: string;
  public sortColumn: string;
  public pageNumber: number;
  public pageSize: number;

  public constructor(init?: Partial<PageParameters>) {
    (<any>Object).assign(this, init);
  }
}

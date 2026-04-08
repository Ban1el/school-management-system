export type BaseDto = {
  dateCreated: Date;
  createdBy: number | null;
  dateModified: Date | null;
  modifiedBy: number | null;
  isActive: boolean;
};

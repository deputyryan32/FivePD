export type PedSearchResponse = Ped[];

export interface Ped {
  NetworkId: number;
  Firstname: string;
  Lastname: string;
  Gender: number;
  Birthdate: {
    Year: number;
    Month: number;
    Day: number;
  }
  /** Indicates if the citizen is wanted by law enforcement. */
  wanted?: boolean;
}
export class BaseDto<T> {
  eventType: string;

  constructor(init?: Partial<T>) {
    this.eventType = this.constructor.name;
    Object.assign(this, init);
  }
}


export class ClientWantsToTextServeDto extends BaseDto<ClientWantsToTextServeDto>{
  message?: string;
  isUser?: boolean;
}

export class ServerRespondsToUser extends BaseDto<ServerRespondsToUser>
{
  message?: string;
  isUser?: boolean;
}

export class ServerReturnsListOfLanguageNames extends BaseDto<ServerReturnsListOfLanguageNames>
{
  names? : Array<string>;
}

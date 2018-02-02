import { DependencyRequest } from "./IoC";

export interface IInjectable {
    readonly $dependencies : DependencyRequest[]
}
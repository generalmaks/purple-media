import {Tweet} from "./tweet";

export interface SearchResult {
  post: Tweet;
  indices: number[];
}

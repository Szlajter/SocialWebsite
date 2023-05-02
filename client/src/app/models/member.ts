import { Photo } from "./photo"

export interface Member {
    id: number
    userName: string
    nickName: string
    photoUrl: string
    age: number
    accountCreationDate: Date
    lastActive: Date
    gender: string
    city: string
    country: string
    description: string
    photos: Photo[]
  }
  
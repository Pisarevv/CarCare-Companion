import axios from 'axios';

const BASE_URL = 'https://localhost:7152';

export default axios.create({
    baseURL: BASE_URL
});

export const axiosPrivate = axios.create({
    baseURL: BASE_URL,
    headers: { 'Content-Type': 'application/json' },
    withCredentials: true
});


export const axiosPrivateFile = axios.create({
    baseURL: BASE_URL,
    withCredentials: true
});

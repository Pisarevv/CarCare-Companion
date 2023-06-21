let target = "https://localhost:7152"

async function request(method, url, data) {
    let options = {
        method,
        headers: {}
    }

    if (data) {
        options.headers["Content-type"] = "application/json";
        options.body = JSON.stringify(data);
    }

    const user = localStorage.getItem('user');
    const auth = JSON.parse(user || '{}');


    if (auth.accessToken) {
        options.headers['X-Authorization'] = auth.accessToken;
    }

    try {
        let response = await fetch(target + url, options);

        if (response.status === 204) {
            return response;
        }

        let result = await response.json();

        if (response.ok !== true) {
            throw (result.message);
        }

        return result;
    }
    catch (error) {  
        throw (error);
    }

}



function post(url, data) {
    return request("post", url, data)
}

function get(url) {
    return request("get", url);
}

function put(url, data) {
    return request("put", url, data)
}

function patch(url, data) {
    return request("patch", url, data)
}

function del(url) {
    return request("delete", url);
}

export {
    post,
    get,
    put,
    patch,
    del as delete
}
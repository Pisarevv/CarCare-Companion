let target = "https://localhost:7152"

async function request(method, url, data) {
    let options = {
        method,
        headers: {}
    }

    if (data){
        options.headers["Content-type"] = "application/json";
        options.body = JSON.stringify(data);
    }
    
    const user = localStorage.getItem('user');
    const auth = JSON.parse(user || '{}');


    if (auth.accessToken) {
        options.headers['Authorization'] = "Bearer " + auth.accessToken;
    }

    try {
        let response = await fetch(target + url, options);

        if (response.status === 204) {
            return response;
        }

        let result = await response.json();

        if (response.ok !== true) {
            throw (result.title);
        }

        return result;
    }
    catch (error) {
        throw (error);
    }

}



function post(url, data, contentType) {
    return request("POST", url, data, contentType)
}

function get(url) {
    return request("GET", url);
}

function put(url, data, contentType) {
    return request("PUT", url, data, contentType)
}

function patch(url, data, contentType) {
    return request("PATCH", url, data, contentType)
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
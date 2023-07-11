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
    
    const userObj = localStorage.getItem('user');
    const user = JSON.parse(userObj || '{}');


    if (user.accessToken) {
        options.headers['Authorization'] = "Bearer " + user.accessToken;
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



function post(url, data) {
    return request("POST", url, data)
}

function get(url) {
    return request("GET", url);
}

function put(url, data) {
    return request("PUT", url, data)
}

function patch(url, data) {
    return request("PATCH", url, data)
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
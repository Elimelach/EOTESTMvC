"use strict"
const setData = (data) => {
    sessionStorage.setItem('data', JSON.stringify(data));
}
const getData = () => { return JSON.parse(sessionStorage.getItem('data')) }

const isOldAvl = () => {
    let o = getData();
    if (o === null || o === undefined) return false;
    return true
}
const showResult = () => {
    let data = getData();
    console.log(data)
    let keys = Object.keys(data);
    let html = ``
    for (let key of keys) {
        html += `<li>${key} - ${data[key]}`;
    }
    document.querySelector('#uli').innerHTML = html;
}
const fetchNew = () => {
 fetch('/home/Launch').then(res => res.json()).then(json => {
        setData(json);
        showResult();
    });
}

if (isOldAvl()) {
    console.log(isOldAvl())
    showResult();
    
}
else {
    fetchNew();
}

document.querySelector('#reload').addEventListener('click', () => {
    fetchNew();
})


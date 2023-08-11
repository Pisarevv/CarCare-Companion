/**
 * Converts a Base64 Data URL into a File object.
 * 
 * @param {string} dataURL - The Base64 Data URL to convert.
 * @param {string} filename - The desired filename for the resulting File object.
 * @returns {File} - A File object representing the given Data URL.
 */

const dataURLtoFile = (dataURL, filename) =>{
    const arr = dataURL.split(',');
    const mime = arr[0].match(/:(.*?);/)[1];
    const bstr = atob(arr[1]);
    let n = bstr.length;
    const u8arr = new Uint8Array(n);
    while (n--) {
      u8arr[n] = bstr.charCodeAt(n);
    }
    return new File([u8arr], filename, { type: mime });
  };


export default dataURLtoFile;
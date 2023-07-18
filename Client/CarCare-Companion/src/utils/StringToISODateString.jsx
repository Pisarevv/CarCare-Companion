/**
 *  StringToISODateString
 *  This function that recieves a string in a format "MM/dd/yyyy" 
 * and converts it to a ISO 8601 datetime string
 * -----------------
**/

const StringToISODateString = (inputString) => {
    const dateTimeStamp = Date.parse(inputString);

    const date = new Date(dateTimeStamp);

    return date.toISOString();
    
}

export default StringToISODateString;
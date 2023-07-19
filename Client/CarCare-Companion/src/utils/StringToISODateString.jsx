/**
 * -----------------
 *  StringToISODateString
 *  This function that recieves a string in a format "dd/MM/yyyy" 
 * and converts it to a ISO 8601 datetime string
 * -----------------
**/


const StringToISODateString = (inputString) => {

    //Changing all '/' with '-' example - 12/03/2023 to 12-03-2023
    const formatedInput = inputString.replaceAll('/','-');

    //Formatting the string in  yyyy/MM/dd format to get the correct time stamp - example - 12-03-2023 to 2023-03-12
    const formattedString = formatedInput.substring(6,10) + formatedInput.substring(2,6) + formatedInput.substring(0,2);

    //Making the correct time stamp for date
    const dateTimeStamp = Date.parse(formattedString);

    //Creating a new date based on the time stamp
    const date = new Date(dateTimeStamp);

    //Returning the date in an ISO 8601 format - example - 2023‐07‐19T06:01:31Z
    return date.toISOString();
    
}

export default StringToISODateString;
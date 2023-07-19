/**
 * -----------------
 *  ISODateStringToString
 *  This function that recieves a string in a ISO 8601 format 
 * and converts it to different formats based on the used function
 * -----------------
**/

import dayjs from "dayjs"


const ISODateStringToString = {
   ddmmyyyy : function(input){
    return dayjs(input).format('DD/MM/YYYY')
   },
}

export default ISODateStringToString;

/**
 * Formats a string containing a number by replacing decimal separators.
 * Specifically, it replaces commas with periods.
 * 
 * @param {string|number} inputString - The string or number to format.
 * @returns {string} - The formatted string with periods as the decimal separator.
 */

const DecimalSeparatorFormatter = (inputString) => {
    inputString = String(inputString);
    if(inputString.includes(',')){
    return inputString.replaceAll(',','.');
    }

    return inputString;
};

export default DecimalSeparatorFormatter;
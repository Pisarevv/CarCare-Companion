const DecimalSeparatorFormatter = (inputString) => {
    inputString = String(inputString);
    if(inputString.includes(',')){
    return inputString.replaceAll(',','.');
    }

    return inputString;
};

export default DecimalSeparatorFormatter;
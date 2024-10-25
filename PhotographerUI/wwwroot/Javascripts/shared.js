function indexOfCollection(collection, selector)
{
    for (var i = 0; i < collection.length; i++)
    {
        if (selector(collection[i]))
        {
            return i;
        }
    }

    return -1;
}


Array.prototype.customAll = function (delegate)
{
    for (var i = 0; i < this.length; i++)
    {
        if (!delegate(this[i], i, this.length))
        {
            return false;
        }
    }

    return true;
}

Array.prototype.customAny = function (delegate)
{
    for (var i = 0; i < this.length; i++)
    {
        if (delegate(this[i], i, this.length))
        {
            return true;
        }
    }

    return false;
}

Array.prototype.customFirst = function (delegate)
{
    for (var i = 0; i < this.length; i++)
    {
        if (delegate(this[i], i, this.length))
        {
            return this[i];
        }
    }

    return null;
}


Array.prototype.customWhere = function (delegate)
{
    var result = [];

    for (var i = 0; i < this.length; i++)
    {
        if (delegate(this[i], i, this.length))
        {
            result.push(this[i]);
        }
    }

    return result;
}

Array.prototype.customSelect = function (delegate)
{
    var result = [];

    for (var i = 0; i < this.length; i++)
    {
        result.push(delegate(this[i], i, this.length));
    }

    return result;
}

Array.prototype.customSelectMany = function (delegate)
{
    var result = [];

    for (var i = 0; i < this.length; i++)
    {
        result = result.concat(delegate(this[i], i));
    }

    return result;
}

Array.prototype.customForEach = function (delegate)
{
    for (var i = 0; i < this.length; i++)
    {
        delegate(this[i], i, this.length);
    }

    return this;
}

String.prototype.format = function ()
{
    var result = this.toString();
    var i = 0;
    for (var arg in arguments)
    {
        if (!args.hasOwnProperty(arg))
        {
            continue;
        }

        var strToReplace = "{" + i++ + "}";
        result = result.replace(strToReplace, (arg || ''));
    }

    return result;
}

String.prototype.formatRegEx = function (clearPattern, formatPattern, formatTemplate)
{
    var clearRe = new RegExp(clearPattern, 'gi');
    var value = this.replace(clearRe, '');

    if (formatPattern && formatTemplate)
    {
        var formatRe = new RegExp(formatPattern, 'gi');
        value = value.replace(formatRe, formatTemplate);
    }

    return value;
}
String.prototype.formatRussianPhone = function ()
{
    var clearAllRe = (/\D/gi);
    var value = this;

    var cleanNumber = value.replace(clearAllRe, '');

    if (cleanNumber.length == 10 && cleanNumber[0] != '7')
    {
        cleanNumber = '7' + '' + cleanNumber;
    }
    
    if (/^[78](90[012345689]|9[1238]\d|941|95[01234568]|96[0-8]|97[12]|99[12345679])/.test(cleanNumber)) // russian phone
    {
        value = cleanNumber.formatRussianMobilePhone();
        return value ? '+' + value : '';
    }
}

String.prototype.formatRussianMobilePhone = function ()
{

    var russianFormatter = Common.formatRegEx('\\D', '^(\\d)(\\d\\d\\d)(\\d\\d\\d)(\\d+)$', '7 ($2) $3-$4');
    
    var fixSpace = (/\s+/gi);

    pThis.formatFunction = function (value)
    {
        

        value = foreignFormatter((value || '').replace(fixSpace, ' '));
        return value ? '+' + value : '';
    };

    return pThis;
};

String.prototype.formatRussianStaticPhone = function ()
{
    var pThis = new Master.Formatter();

    var cases =
        [
            {
                regex: /^8\((\d+)\)(\d\d)(\d\d)$/,
                template: '8 ($1) $2-$3'
            },
            {
                regex: /^8\((\d+)\)(\d)(\d\d)(\d\d)$/,
                template: '8 ($1) $2-$3-$4'
            },
            {
                regex: /^8\((\d+)\)(\d\d)(\d\d)(\d\d)$/,
                template: '8 ($1) $2-$3-$4'
            },
            {
                regex: /^8\((\d+)\)(\d\d\d)(\d+)$/,
                template: '8 ($1) $2-$3'
            },
            {
                regex: /^8(49[5689]|812|800)(\d\d\d)(\d+)$/,
                template: '8 ($1) $2-$3'
            }
        ];

    var clearRe = (/[^\d\(\)]/gi);

    pThis.formatFunction = function (value)
    {
        var cleanNumber = (value || '').replace(clearRe, '');

        if (cleanNumber.length > 5 && cleanNumber[0] != '8')
        {
            cleanNumber = '8' + '' + cleanNumber;
        }

        for (var i = 0; i < cases.length; i++)
        {
            if (cases[i].regex.test(cleanNumber))
            {
                value = cleanNumber.replace(cases[i].regex, cases[i].template);
                break;
            }
        }

        if (value && value.length > 0 && value[0] == '+')
        {
            value = value.substring(1, value.length);
        }

        return value;
    };

    return pThis;
};

function updateCollectionOld(collection, updatedEntry, keySelector, updateProperties, newEntryCallback, updateCallback)
{
    var targetEntry = null;

    for (var i = 0; i < collection.length; i++)
    {
        if (keySelector(collection[i]) == keySelector(updatedEntry))
        {
            targetEntry = collection[i];
        }
    }

    if (targetEntry == null)
    {
        if (newEntryCallback)
        {
            if (newEntryCallback(updatedEntry))
            {
                collection.splice(0, 0, updatedEntry);
            }
        }
        else
        {
            collection.splice(0, 0, updatedEntry);
        }
    }
    else
    {
        if (updateProperties)
        {
            for (var i = 0; i < updateProperties.length; i++)
            {
                var property = updateProperties[i];

                if (updatedEntry.hasOwnProperty(property))
                {
                    targetEntry[property] = updatedEntry[property];
                }
            }
        }

        if (updateCallback)
        {
            updateCallback(targetEntry);
        }
    }
}

function updateCollection(params)
{
    var targetEntry = null;

    for (var i = 0; i < params.collection.length; i++)
    {
        if (params.keySelector)
        {
            if (params.keySelector(params.collection[i]) == params.keySelector(params.updatedEntry))
            {
                targetEntry = params.collection[i];
                break;
            }
        }
        else if (params.entrySelector)
        {
            if (params.entrySelector(params.collection[i]))
            {
                targetEntry = params.collection[i];
                break;
            }
        }
        else
        {
            return;
        }
    }

    if (targetEntry == null)
    {
        if (params.newEntryCallback)
        {
            if (params.newEntryCallback(params.updatedEntry))
            {
                params.collection.splice(0, 0, params.updatedEntry);
            }
        }
        else
        {
            params.collection.splice(0, 0, params.updatedEntry);
        }
    }
    else
    {
        if (params.updateProperties)
        {
            for (var i = 0; i < params.updateProperties.length; i++)
            {
                var property = params.updateProperties[i];

                if (params.updatedEntry.hasOwnProperty(property))
                {
                    targetEntry[property] = params.updatedEntry[property];
                }
            }
        }

        if (params.updateCallback)
        {
            params.updateCallback(targetEntry);
        }
    }
}

function deleteFromCollection(collection, selector, doOnce)
{
    for (var i = 0; i < collection.length; i++)
    {
        if (selector(collection[i]))
        {
            collection.splice(i, 1);

            if (doOnce)
            {
                return;
            }
        }
    }
}


function startWithDelay (code, ms, conditionTrigger) {
    var timer = null;

    var run = function () {
        var args = arguments;
        var pThis = this;

        if (timer) {
            clearTimeout(timer);
        }

        timer = setTimeout(
            function () {
                if (conditionTrigger && !conditionTrigger()) {
                    run.apply(pThis, args);
                    return;
                }

                code.apply(pThis, args);
            },
            ms);
    }

    return run;
}

function newGuid()
{
    function s4()
    {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }

    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
}

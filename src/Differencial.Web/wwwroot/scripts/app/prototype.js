
if (!Array.prototype.findIndex) {
    Array.prototype.findIndex = function (predicate) {
        if (this === null) {
            throw new TypeError('Array.prototype.findIndex called on null or undefined');
        }
        if (typeof predicate !== 'function') {
            throw new TypeError('predicate must be a function');
        }
        var list = Object(this);
        var length = list.length >>> 0;
        var thisArg = arguments[1];
        var value;

        for (var i = 0; i < length; i++) {
            value = list[i];
            if (predicate.call(thisArg, value, i, list)) {
                return i;
            }
        }
        return -1;
    };
}

if (!Array.prototype.move) {
    Array.prototype.move = function (old_index, new_index) {
        if (new_index >= this.length) {
            var k = new_index - this.length;
            while ((k--) + 1) {
                this.push(undefined);
            }
        }
        this.splice(new_index, 0, this.splice(old_index, 1)[0]);
        return this; // for testing purposes
    };
}

if (!Array.prototype.find) {
    Array.prototype.find = function (predicate) {
        if (this === null) {
            throw new TypeError('Array.prototype.find called on null or undefined');
        }
        if (typeof predicate !== 'function') {
            throw new TypeError('predicate must be a function');
        }
        var list = Object(this);
        var length = list.length >>> 0;
        var thisArg = arguments[1];
        var value;

        for (var i = 0; i < length; i++) {
            value = list[i];
            if (predicate.call(thisArg, value, i, list)) {
                return value;
            }
        }
        return undefined;
    };
}

if (!String.prototype.replaceAll) {
    String.prototype.replaceAll = function (target, replacement) {
        return this.split(target).join(replacement);
    };
}

jQuery.fn.extend({
    disable: function () {
        return this.each(function () {
            jQuery(this).attr("disabled", "disabled");
        });
    },
    enable: function () {
        return this.each(function () {
            jQuery(this).removeAttr("disabled");
        });
    },

});

if (!HTMLCollection.prototype.removeAll) {
    HTMLCollection.prototype.removeAll = function () {
        if (this === null) {
            throw new TypeError('HTMLOptionsCollection.prototype.removeAll called on null or undefined');
        }
        var len = this.length;
        for (var i = 0; i <= len ; i++)
            this.remove(0);
        return;
    };
}

if (!String.prototype.toIntOrZero) {
    String.prototype.toIntOrZero = function () {
        console.log(this);
        if (isNaN(parseInt(this)) == false)
            return parseInt(this);
        else
            return 0;

    };
}
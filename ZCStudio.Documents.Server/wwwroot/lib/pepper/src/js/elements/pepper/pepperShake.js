(function($) {
    'use strict';

    /**
     * Shake an Element
     *
     * @param distance Distance in px to shake
     * @param runs How many times it shakes
     * @param speed Speed of the shake
     * @param distanceReduce Reduction of the distance for each following shake
     */
    $.fn.pepperShake = function(distance, runs, speed, distanceReduce) {
        var _distance = 10;
        var _speed = 100;
        var _runs = 3;
        var _distanceReduce = 0;

        if(isDefined(distance)) { _distance = distance; }
        if(isDefined(runs)) { _runs = runs; }
        if(isDefined(speed)) { _speed = speed; }
        if(isDefined(distanceReduce)) { _distanceReduce = distanceReduce; }

        var position = this.css('position');

        if(position == 'static') {
            this.css('position', 'relative');
        }

        for (var i = 0; i <= _runs; ++i) {
            if(i == 0) {
                this.animate({ left: '-' + _distance + 'px' }, _speed / 2).animate({ left: _distance + 'px' }, _speed);
            } else if (i == _runs) {
                this.animate({ left: '0px' }, _speed / 2, function() {
                    $(this).css('position', '').css('left', '');
                });
            } else {
                this.animate({ left: '-' + _distance + 'px' }, _speed).animate({ left: _distance + 'px' }, _speed);
            }
            _distance -= _distanceReduce;
        }
    };

}(jQuery));


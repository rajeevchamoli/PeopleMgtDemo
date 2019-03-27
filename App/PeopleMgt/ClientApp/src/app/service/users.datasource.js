"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var rxjs_1 = require("rxjs");
var UsersDataSource = /** @class */ (function () {
    function UsersDataSource(userService) {
        this.userService = userService;
        this.usersSubject = new rxjs_1.BehaviorSubject([]);
    }
    UsersDataSource.prototype.loadUsers = function (nav) {
        var _this = this;
        this.loading = true;
        this.userService.getUsers(nav).subscribe(function (resp) {
            // get  Paging-Headers from response headers
            var pagingData = resp.headers.get('Paging-Headers');
            _this.pagingMetadata = JSON.parse(pagingData);
            // access the body directly.
            var userArray = resp.body;
            _this.usersSubject.next(userArray);
            _this.userCount = userArray.length;
            _this.loading = false;
        });
    };
    UsersDataSource.prototype.connect = function (collectionViewer) {
        console.log("Connecting data source");
        return this.usersSubject.asObservable();
    };
    UsersDataSource.prototype.disconnect = function (collectionViewer) {
        this.usersSubject.complete();
    };
    return UsersDataSource;
}());
exports.UsersDataSource = UsersDataSource;
//# sourceMappingURL=users.datasource.js.map

(function (cr) {
    var AccountLoginViewModel = function (returnUrl) {

        var self = this;

        self.viewModelHelper = new CarRental.viewModelHelper();
        self.accountModel = new CarRental.AccountLoginModel();

        self.login = function (model) {
            var errors = ko.validation.group(model);
            self.viewModelHelper.modelIsValid(model.isValid());
            if (errors().length == 0) {

            }
            else
                self.viewModelHelper.modelErrors(errors());
        }
    }
    cr.AccountLoginViewModel = AccountLoginViewModel;
}(window.CarRental));

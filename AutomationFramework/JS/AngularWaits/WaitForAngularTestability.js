// Wait function until AngularJS Testability would complete
function waitForAngularTestability(callback) {
	window.angular.getTestability(document).whenStable(function (didWork) {
		callback(true);
	});
}

// Callback function
waitForAngularTestability(function (isReady) {
	window.angularIsTestable = true;
})
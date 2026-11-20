// UI helpers only.
// Product data, search, cart, wishlist, and forms are left for ASP.NET Core MVC.

(function () {
  var button = document.querySelector(".back-to-top");

  if (!button) {
    return;
  }

  window.addEventListener("scroll", function () {
    if (window.scrollY > 400) {
      button.classList.add("is-visible");
    } else {
      button.classList.remove("is-visible");
    }
  });

  button.addEventListener("click", function () {
    var reduceMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
    window.scrollTo({
      top: 0,
      behavior: reduceMotion ? "auto" : "smooth"
    });
  });
})();

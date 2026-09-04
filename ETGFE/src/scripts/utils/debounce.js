export default function (func, wait = 120, immediate) {
  let timeout;
  return function (...args) {
    let later = () => {
      timeout = null;
      if (!immediate) func.apply(this, args);
    };

    let callNow = immediate && !timeout;
    clearTimeout(timeout);
    timeout = setTimeout(later, wait);
    if (callNow) func.apply(this, args);
  };
}

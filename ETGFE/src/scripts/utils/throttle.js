export default function throttle(func, interval) {
  let lastCallTime = 0;

  return function (...args) {
    const now = new Date().getTime();
    if (now - lastCallTime >= interval) {
      func.apply(this, args);
      lastCallTime = now;
    }
  };
}

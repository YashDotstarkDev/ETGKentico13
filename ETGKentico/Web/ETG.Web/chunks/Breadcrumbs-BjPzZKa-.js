import { c as createComponent, m as maybeRenderHead, r as renderTemplate } from './astro/server-BySIDU-D.js';
import 'kleur/colors';
import 'html-escaper';
import 'clsx';

const $$Breadcrumbs = createComponent(($$result, $$props, $$slots) => {
  return renderTemplate`<!--
/////
breadcrumbs Start
/////
-->${maybeRenderHead()}<div class="breadcrumbs py-4"> <div class="wrapper-large"> <div class="flex items-center gap-1 md:gap-2 text-xs md:text-sm"> <a href="/" class="shrink-0 hover:no-underline">Home</a> <div class="divider shrink-0">/</div> <a href="#" class="shrink-0 hover:no-underline">Primary</a> <div class="divider shrink-0">/</div> <a href="#" class="shrink-0 hover:no-underline">Secondary</a> <div class="divider shrink-0">/</div> <div class="whitespace-nowrap overflow-hidden text-ellipsis">Current lorem ipsum dolor</div> </div> </div> </div> <!--
/////
breadcrumbs End
/////
-->`;
}, "D:/Projects/Devotion/ETG/etg-astro/src/components/Breadcrumbs.astro", void 0);

export { $$Breadcrumbs as $ };

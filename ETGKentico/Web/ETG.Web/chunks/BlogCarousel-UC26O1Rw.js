import { c as createComponent, m as maybeRenderHead, a as renderComponent, r as renderTemplate } from './astro/server-BySIDU-D.js';
import 'kleur/colors';
import 'html-escaper';
import { $ as $$PackageTile } from './CustomerReviews-DPDsoxHs.js';
import 'clsx';

const $$TourTilesCollection = createComponent(($$result, $$props, $$slots) => {
  return renderTemplate`<!--
/////
tour-tiles-collection start
/////
-->${maybeRenderHead()}<div class="tour-tiles-collection"> <div class="wrapper"> <h2 class="text-center uppercase text-black">Our featured packages</h2> <div class="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 mt-12 gap-x-6 gap-y-12"> ${renderComponent($$result, "PackageTile", $$PackageTile, {})} ${renderComponent($$result, "PackageTile", $$PackageTile, {})} ${renderComponent($$result, "PackageTile", $$PackageTile, {})} ${renderComponent($$result, "PackageTile", $$PackageTile, {})} ${renderComponent($$result, "PackageTile", $$PackageTile, {})} ${renderComponent($$result, "PackageTile", $$PackageTile, {})} </div> <div class="cta mt-12 text-center"> <a href="#" class="btn btn-primary-outline">View all</a> </div> </div> </div> <!--
/////
tour-tiles-collection end
/////
-->`;
}, "C:/Projects/ETG/Source/_cut-etgp0001-entire-travel-group/src/components/TourTilesCollection.astro", void 0);

const $$ArticleTile = createComponent(($$result, $$props, $$slots) => {
  return renderTemplate`${maybeRenderHead()}<a href="#" class="block hover:no-underline relative text-body group"> <span class="block image-container w-full h-[180px] relative rounded-md overflow-hidden isolate"> <img data-lowsrc="https://entiretravel.imgix.net/getmedia/bcc3edea-2369-4774-b62a-a6669554644a/Group-Journeys-275x400.jpg?auto=format&w=3" data-src="https://entiretravel.imgix.net/getmedia/bcc3edea-2369-4774-b62a-a6669554644a/Group-Journeys-275x400.jpg?auto=format&w={width}" data-sizes="auto" class="lazyload absolute z-0 inset-0 w-full h-full object-cover group-hover:scale-105 transition-all duration-300" alt="Hilton Moorea Lagoon Resort & Spa"> </span> <span class="flex flex-col justify-end gap-2 py-4 relative z-20"> <span class="text-xs font-bold uppercase">Palau</span> <span class="text-lg text-primary group-hover:text-body transition-colors duration-300 font-normal">Cultural Encounters: Immersing yourself in Palauan traditions</span> </span> </a>`;
}, "C:/Projects/ETG/Source/_cut-etgp0001-entire-travel-group/src/components/ArticleTile.astro", void 0);

const $$BlogCarousel = createComponent(($$result, $$props, $$slots) => {
  return renderTemplate`<!--
/////
blog-carousel Start
/////
-->${maybeRenderHead()}<div class="blog-carousel"> <div class="wrapper"> <h2 class="text-black text-center uppercase">Travel blogs &amp; guides</h2> <div class="prose text-body text-lg md:text-xl text-center mt-6 mx-auto max-w-5xl text-balance"> <p>Looking for more travel inspiration... We've got you covered.</p> <p>Explore our extensive range of travel blogs including the Canadian Rockies, Tropical Island escapes, immersive experiences across Europe and so much more.</p> </div> <div class="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-4 gap-8 mt-8 items-start"> ${renderComponent($$result, "ArticleTile", $$ArticleTile, {})} ${renderComponent($$result, "ArticleTile", $$ArticleTile, {})} ${renderComponent($$result, "ArticleTile", $$ArticleTile, {})} ${renderComponent($$result, "ArticleTile", $$ArticleTile, {})} </div> <div class="cta mt-8 flex justify-center"> <a href="#" class="btn btn-primary-outline">View all</a> </div> <!--<div class="-mx-4 mt-6 2xl:-mx-16">--> <!--    <TilesCarousel client:only="vue">--> <!--        <div class="relative px-6 lg:px-16">--> <!--            <div class="swiper isolate text-left text-sm font-semibold relative z-10">--> <!--                <div class="swiper-wrapper">--> <!--                    <div class="swiper-slide">--> <!--                        <ArticleTile />--> <!--                    </div>--> <!--                    <div class="swiper-slide">--> <!--                        <ArticleTile />--> <!--                    </div>--> <!--                    <div class="swiper-slide">--> <!--                        <ArticleTile />--> <!--                    </div>--> <!--                    <div class="swiper-slide">--> <!--                        <ArticleTile />--> <!--                    </div>--> <!--                    <div class="swiper-slide">--> <!--                        <ArticleTile />--> <!--                    </div>--> <!--                    <div class="swiper-slide">--> <!--                        <ArticleTile />--> <!--                    </div>--> <!--                </div>--> <!--            </div>--> <!--            <div class="button-prev cursor-pointer rounded-full w-[50px] h-[50px] bg-primary text-white hover:bg-body flex items-center justify-center text-lg transition-colors duration-300 absolute left-0 top-[65px] z-20">--> <!--                <i class="fas fa-chevron-left"></i>--> <!--            </div>--> <!--            <div class="button-next cursor-pointer rounded-full w-[50px] h-[50px] bg-primary text-white hover:bg-body flex items-center justify-center text-lg transition-colors duration-300 absolute right-0 top-[65px] z-20">--> <!--                <i class="fas fa-chevron-right"></i>--> <!--            </div>--> <!--        </div>--> <!--    </TilesCarousel>--> <!--</div>--> </div> </div> <!--
/////
blog-carousel End
/////
-->`;
}, "C:/Projects/ETG/Source/_cut-etgp0001-entire-travel-group/src/components/BlogCarousel.astro", void 0);

export { $$TourTilesCollection as $, $$BlogCarousel as a };
